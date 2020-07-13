using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using TaskList.Core.Constants;
using TaskList.Core.DTOs;
using TaskList.Data.Model;

namespace TaskList.Business.Helpers
{
    public static class Tasks
    {
        public static TaskDTO GetByID(Guid id)
        {
            using (var db = new DB())
            {
                var dto = db.Task
                    .Where(x => x.TaskId == id)
                    .Select(x => new TaskDTO()
                    {
                        TaskId = x.TaskId,
                        Name = x.Name,
                        AreaId = x.Area.AreaId,
                        AreaName = x.Area.Name,
                        SubAreaId = x.SubArea.SubAreaId,
                        SubAreaName = x.SubArea.Name,
                        FrequencyId = x.Frequency.FrequencyId,
                        FrequencyName = x.Frequency.Name,
                        Notes = x.Notes,
                        IsInPolicyTech = x.IsInPolicyTech,
                        ProcedureFileName = x.ProcedureFileName,
                        DisplayOrder = x.DisplayOrder,
                        IsActive = x.IsActive,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate,
                    })
                    .FirstOrDefault();

                return dto;
            }
        }

        public static TaskDetailsDTO GetByIDWithDetails(Guid id)
        {
            using (var db = new DB())
            {
                var dto = db.Task
                    .Where(x => x.TaskId == id)
                    .Select(x => new TaskDetailsDTO()
                    {
                        TaskId = x.TaskId,
                        Name = x.Name,
                        AreaId = x.Area.AreaId,
                        AreaName = x.Area.Name,
                        SubAreaId = x.SubArea.SubAreaId,
                        SubAreaName = x.SubArea.Name,
                        FrequencyId = x.Frequency.FrequencyId,
                        FrequencyName = x.Frequency.Name,
                        Notes = x.Notes,
                        IsInPolicyTech = x.IsInPolicyTech,
                        ProcedureFileName = x.ProcedureFileName,
                        DisplayOrder = x.DisplayOrder,
                        IsActive = x.IsActive,
                        TaskStaff = x.Staff.Select(s => new TaskStaffDTO()
                        {
                            UserId = s.UserId,
                            StaffId = s.StaffId,
                            TaskId = s.TaskId,
                            StaffTypeId = s.StaffTypeId,
                            StaffTypeName = s.StaffType.Name,
                            Name = $"{s.User.LastName}, {s.User.FirstName}"
                        }),
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate,
                    })
                    .FirstOrDefault();

                return dto;
            }
        }

        public static List<TaskDTO> GetAll(bool activeOnly = false)
        {
            using (var db = new DB())
            {
                IQueryable<Task> query = db.Task
                    .Include(x => x.Staff)
                        .ThenInclude(x => x.User);

                if (activeOnly)
                {
                    query = query.Where(x => x.IsActive);
                }

                var dtos = query
                    .Select(x => new TaskDTO()
                    {
                        RowKey = x.TaskId,
                        TaskId = x.TaskId,
                        Name = x.Name,
                        AreaId = x.Area.AreaId,
                        AreaName = x.Area.Name,
                        SubAreaId = x.SubArea.SubAreaId,
                        SubAreaName = x.SubArea.Name,
                        FrequencyId = x.Frequency.FrequencyId,
                        FrequencyName = x.Frequency.Name,
                        Notes = x.Notes,
                        IsInPolicyTech = x.IsInPolicyTech,
                        ProcedureFileName = x.ProcedureFileName,
                        PrimaryStaffName = x.Staff
                            .Where(s => s.StaffType.Name == StaffTypeNames.Primary)
                            .Select(x => $"{x.User.LastName}, {x.User.FirstName}")
                            .FirstOrDefault(),
                        DisplayOrder = x.DisplayOrder,
                        IsActive = x.IsActive,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate
                    });

                return dtos
                    .OrderBy(x => x.DisplayOrder)
                    .ToList();
            }
        }

        public static Guid? Save(TaskDetailsDTO toSave, string currentUserEmail)
        {
            using (var db = new DB())
            {
                try
                {
                    Task entity = db.Task
                        .Include(x => x.Staff).ThenInclude(s => s.StaffType)
                        .Where(x => x.TaskId == toSave.TaskId)
                        .FirstOrDefault();

                    if (entity == null)
                    {
                        if (toSave.TaskId == Guid.Empty)
                        {
                            entity = new Task();
                            entity.TaskId = Guid.NewGuid();
                        }
                        else
                        {
                            entity = new Task();
                            entity.TaskId = toSave.TaskId;
                        }

                        entity.CreatedDate = DateTime.Now;
                        entity.CreatedBy = currentUserEmail;
                        db.Task.Add(entity);
                    }

                    entity.Name = toSave.Name;
                    entity.AreaId = toSave.AreaId;
                    entity.SubAreaId = toSave.SubAreaId;
                    entity.FrequencyId = toSave.FrequencyId;
                    entity.Notes = toSave.Notes;
                    entity.IsInPolicyTech = toSave.IsInPolicyTech;
                    entity.ProcedureFileName = toSave.ProcedureFileName;
                    entity.DisplayOrder = toSave.DisplayOrder;
                    entity.IsActive = toSave.IsActive;
                    entity.UpdatedDate = DateTime.Now;
                    entity.UpdatedBy = currentUserEmail;

                    db.SaveChanges();

                    return entity.TaskId;
                }
                catch (Exception ex)
                {
                    throw new Exception("Exception thrown in Tasks.Save");
                }
            }
        }

        public static void Toggle(Guid id)
        {
            using (var db = new DB())
            {
                try
                {
                    Task entity = db.Task
                        .Where(x => x.TaskId == id)
                        .FirstOrDefault();

                    entity.IsActive = !entity.IsActive;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    // ex.Publish();
                    throw new Exception("Exception thrown in Tasks.Toggle");
                }
            }
        }

        public static List<TaskDTO> GetAllTasks()
        {
            using (var db = new DB())
            {
                // Get all tasks, matching on staff, but including tasks without staff (left joins)
                IQueryable<TaskDTO> dtos = from task in db.Task
                                           join stf in db.Staff on task.TaskId equals stf.TaskId into _staff
                                           from staff in _staff.DefaultIfEmpty()
                                           join usr in db.User on staff.UserId equals usr.UserId into _user
                                           from user in _user.DefaultIfEmpty()
                                           join stfType in db.StaffType on staff.StaffTypeId equals stfType.StaffTypeId into _staffType
                                           from staffType in _staffType.DefaultIfEmpty()
                                           where staffType == null ? true : staffType.Name == StaffTypeNames.Primary
                                           select new TaskDTO()
                                           {
                                               RowKey = staff.StaffId,
                                               TaskId = task.TaskId,
                                               Name = task.Name,
                                               AreaId = task.Area.AreaId,
                                               AreaName = task.Area.Name,
                                               SubAreaId = task.SubArea.SubAreaId,
                                               SubAreaName = task.SubArea.Name,
                                               FrequencyId = task.Frequency.FrequencyId,
                                               FrequencyName = task.Frequency.Name,
                                               Notes = task.Notes,
                                               IsInPolicyTech = task.IsInPolicyTech,
                                               ProcedureFileName = task.ProcedureFileName,
                                               PrimaryStaffName = user != null ? $"{user.LastName}, {user.FirstName}" : null,
                                               DisplayOrder = task.DisplayOrder,
                                               IsActive = task.IsActive,
                                               CreatedBy = task.CreatedBy,
                                               CreatedDate = task.CreatedDate,
                                               UpdatedBy = task.UpdatedBy,
                                               UpdatedDate = task.UpdatedDate,
                                           };

                return dtos
                    .OrderBy(x => x.DisplayOrder)
                    .ToList();
            }
        }

        public static List<TaskDTO> GetAllUsersTasks(Guid? userId)
        {
            using (var db = new DB())
            {
                var dtos = db.Staff
                    .Include(x => x.Task.Area)
                    .Include(x => x.Task.SubArea)
                    .Include(x => x.Task.Frequency)
                    .Where(x => x.UserId == userId)
                    .Select(x => new TaskDTO()
                    {
                        RowKey = x.StaffId,
                        TaskId = x.TaskId,
                        Name = x.Task.Name,
                        AreaId = x.Task.Area.AreaId,
                        AreaName = x.Task.Area.Name,
                        SubAreaId = x.Task.SubArea.SubAreaId,
                        SubAreaName = x.Task.SubArea.Name,
                        FrequencyId = x.Task.Frequency.FrequencyId,
                        FrequencyName = x.Task.Frequency.Name,
                        Notes = x.Task.Notes,
                        IsInPolicyTech = x.Task.IsInPolicyTech,
                        ProcedureFileName = x.Task.ProcedureFileName,
                        PrimaryStaffName = x.Task.Staff
                            .Where(s => s.StaffType.Name == StaffTypeNames.Primary).Select(x => x.User.LastName + ", " + x.User.FirstName)
                            .FirstOrDefault(),
                        DisplayOrder = x.Task.DisplayOrder,
                        IsActive = x.Task.IsActive,
                        CreatedBy = x.Task.CreatedBy,
                        CreatedDate = x.Task.CreatedDate,
                        UpdatedBy = x.Task.UpdatedBy,
                        UpdatedDate = x.Task.UpdatedDate
                    });

                return dtos
                    .OrderBy(t => t.DisplayOrder)
                    .ToList();
            }
        }
    }
}