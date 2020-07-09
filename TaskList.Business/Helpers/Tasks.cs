using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
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
                            Name = s.User.LastName + ", " + s.User.FirstName
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
                var dtos = db.Task
                    .Include(x => x.Staff).ThenInclude(x => x.User)
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
                            .Where(s => s.StaffType.Name == StaffTypeNames.Primary).Select(x => x.User.LastName + ", " + x.User.FirstName)
                            .FirstOrDefault(),
                        AssignedStaffMembers = x.Staff.Select(s => new StaffMemberDTO()
                        {
                            StaffTypeId = s.StaffTypeId,
                            StaffTypeName = s.StaffType.Name,
                            FirstName = s.User.FirstName,
                            LastName = s.User.LastName,
                            UserId = s.UserId,
                            IsSupervisor = s.User.IsSupervisor,
                        }),
                        DisplayOrder = x.DisplayOrder,
                        IsActive = x.IsActive,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDate = x.UpdatedDate
                    });

                if (activeOnly)
                {
                    dtos = dtos.Where(x => x.IsActive);
                }

                return dtos
                    .OrderBy(x => x.DisplayOrder)
                    .ToList();
            }
        }

        public static Guid? Save(TaskDetailsDTO toSave)
        {
            // TODO: Should be current user
            var tempEmail = "hscheffert@qci.com";

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
                        entity.CreatedBy = tempEmail;
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
                    entity.UpdatedBy = tempEmail;

                    db.SaveChanges();

                    Tasks.CreateUpdateTaskStaff(toSave, entity.TaskId);

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

        private static void CreateUpdateTaskStaff(TaskDetailsDTO taskDetails, Guid taskId)
        {
            var grouped = taskDetails.TaskStaff
                .GroupBy(x => x.UserId == null)
                .ToDictionary(g => g.Key, g => g.ToList());
            var unassignedStaff = grouped[true];
            var assignedStaff = grouped[false];

            // Delete any Staff that are now unassigned
            List<TaskStaffDTO> staffToDelete = unassignedStaff
                .Where(x => x.StaffId != null)
                .ToList();
            staffToDelete.ForEach(x => Staffs.Delete((Guid)x.StaffId));

            // Create/Update other staff
            var staffDtos = assignedStaff
                .Select(x => new StaffDTO()
                {
                    StaffId = (Guid)(x.StaffId != null ? x.StaffId : Guid.Empty),
                    StaffTypeId = (Guid)x.StaffTypeId,
                    UserId = (Guid)x.UserId,
                    TaskId = taskId,
                    IsActive = taskDetails.IsActive
                });

            foreach (StaffDTO dto in staffDtos)
            {
                Staffs.Save(dto);
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
                        AssignedStaffMembers = x.Task.Staff.Select(s => new StaffMemberDTO()
                        {
                            StaffTypeId = s.StaffTypeId,
                            StaffTypeName = s.StaffType.Name,
                            FirstName = s.User.FirstName,
                            LastName = s.User.LastName,
                            UserId = s.UserId,
                            IsSupervisor = s.User.IsSupervisor,
                        }),
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
