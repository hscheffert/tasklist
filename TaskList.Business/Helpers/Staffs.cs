using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.Core.Constants;
using TaskList.Core.DTOs;
using TaskList.Core.Exceptions;
using TaskList.Data.Model;

namespace TaskList.Business.Helpers
{
    public static class Staffs
    {
        public static StaffDTO GetByID(Guid id)
        {
            using (var db = new DB())
            {
                var dto = db.Staff
                    .Where(x => x.StaffId == id)
                    .Select(x => new StaffDTO()
                    {
                        StaffId = x.StaffId,
                        StaffTypeId = x.StaffTypeId,
                        TaskId = x.TaskId,
                        UserId = x.UserId,
                        TaskName = x.Task.Name,
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName,
                        StaffTypeName = x.StaffType.Name,
                        IsActive = x.IsActive,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate
                    })
                    .FirstOrDefault();

                return dto;
            }
        }

        public static List<StaffDTO> GetAll(bool activeOnly = false)
        {
            using (var db = new DB())
            {
                IQueryable<Staff> query = db.Staff;

                if (activeOnly)
                {
                    query = query.Where(x => x.IsActive);
                }

                var dtos = query
                    .Select(x => new StaffDTO()
                    {
                        StaffId = x.StaffId,
                        StaffTypeId = x.StaffTypeId,
                        TaskId = x.TaskId,
                        UserId = x.UserId,
                        TaskName = x.Task.Name,
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName,
                        StaffTypeName = x.StaffType.Name,
                        IsActive = x.IsActive,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate
                    });

                return dtos
                    .ToList();
            }
        }

        public static Guid? Save(StaffDTO toSave, string currentUserEmail)
        {
            using (var db = new DB())
            {
                try
                {
                    Staff entity = db.Staff
                        .Where(x => x.StaffId == toSave.StaffId)
                        .FirstOrDefault();

                    var currentTaskStaff = db.Staff
                        .Where(x => x.TaskId == toSave.TaskId);
                    var duplicateEntriesForThisUser = currentTaskStaff
                        .Where(x => x.UserId == toSave.UserId);

                    // Check for duplicate entries for this user/task
                    if(duplicateEntriesForThisUser.Any())
                    {
                        throw new EndUserException("User is already included in this Task Staff.");                        
                    }

                    // Make sure everything is active
                    TaskDTO task = Tasks.GetByID(toSave.TaskId);
                    if (!task.IsActive)
                    {
                        throw new Exception("Task " + task.Name + " is inactive.");
                    }

                    StaffTypeDTO staffType = StaffTypes.GetByID(toSave.StaffTypeId);
                    if(!staffType.IsActive)
                    {
                        throw new Exception("Staff type " + staffType.Name + " is inactive.");
                    }                    

                    UserDTO user = Users.GetByID(toSave.UserId);
                    if (!user.IsActive)
                    {
                        throw new Exception("User " + user.Name + " is inactive.");
                    }

                    // User has match the staff type role
                    if (staffType.IsSupervisor && !user.IsSupervisor)
                    {
                        throw new EndUserException("User must be a supervisor to be of Type " + staffType.Name);
                    }

                    // Check for duplicate entries for this staff type (if they are not allowed)
                    if (!staffType.AllowMultiple)
                    {
                        var existingStaffOfThisType = currentTaskStaff
                           .Where(x => x.StaffTypeId == toSave.StaffTypeId);

                        if (existingStaffOfThisType.Any())
                        {
                            throw new EndUserException("Staff type of " + staffType.Name + " does not allow multiple members.");
                        }
                    }

                    if (entity == null)
                    {
                        if(toSave.StaffId == Guid.Empty)
                        {
                            entity = new Staff();
                            entity.StaffId = Guid.NewGuid();
                        }
                        else
                        {
                            entity = new Staff();
                            entity.StaffId = toSave.StaffId;
                        }

                        entity.CreatedDate = DateTime.Now;
                        entity.CreatedBy = currentUserEmail;
                        db.Staff.Add(entity);
                    }                   

                    entity.StaffTypeId = toSave.StaffTypeId;
                    entity.TaskId = toSave.TaskId;
                    entity.UserId = toSave.UserId;
                    entity.IsActive = toSave.IsActive;
                    entity.UpdatedDate = DateTime.Now;
                    entity.UpdatedBy = currentUserEmail;
                    db.SaveChanges();

                    return entity.StaffId;
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public static void Toggle(Guid id)
        {
            using (var db = new DB())
            {
                try
                {
                    Staff entity = db.Staff
                        .Where(x => x.StaffId == id)
                        .FirstOrDefault();

                    entity.IsActive = !entity.IsActive;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    // ex.Publish();
                    throw new Exception("Exception thrown in Staffs.Toggle");
                }
            }
        }

        public static void Delete(Guid id)
        {
            using (var db = new DB())
            {
                try
                {
                    Staff entity = db.Staff
                        .Where(x => x.StaffId == id)
                        .FirstOrDefault();

                    if(entity != null)
                    {
                        db.Staff.Remove(entity);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public static List<TaskStaffDTO> GetAllByTaskId(Guid taskId)
        {
            using (var db = new DB())
            {
                IQueryable<Staff> query = db.Staff
                    .Where(x => x.TaskId == taskId);
               
                var dtos = query
                    .Select(x => new TaskStaffDTO()
                    {
                        StaffId = x.StaffId,
                        StaffTypeId = x.StaffTypeId,
                        TaskId = x.TaskId,
                        UserId = x.UserId,
                        StaffTypeName = x.StaffType.Name,
                        Name = x.User.LastName + ", " + x.User.FirstName,
                        IsActive = x.IsActive,
                    });

                return dtos
                    .ToList();
            }
        }

        public static ILookup<string, string> GetSecondaryTaskStaffLookup(Guid taskId)
        {
            using (var db = new DB())
            {
                return db.Staff
                    .Where(x => x.TaskId == taskId)
                    .Where(x => x.StaffType.Name != StaffTypeNames.Primary)
                    .Select(x => new TaskStaffDTO()
                    {
                        StaffId = x.StaffId,
                        StaffTypeId = x.StaffTypeId,
                        TaskId = x.TaskId,
                        UserId = x.UserId,
                        StaffTypeName = x.StaffType.Name,
                        Name = $"{x.User.LastName}, {x.User.FirstName}",
                        IsActive = x.IsActive,
                    })
                    .ToList()
                    .ToLookup(x => x.StaffTypeName, x => x.Name);
            }
        }
    }
}
