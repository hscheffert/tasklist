using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.Core.DTOs;
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
                var dtos = db.Staff
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

                if (activeOnly)
                {
                    dtos = dtos.Where(x => x.IsActive);
                }

                return dtos.ToList();
            }
        }

        public static Guid? Save(StaffDTO toSave)
        {
            // TODO: Should be current user
            var tempEmail = "hscheffert@qci.com";

            using (var db = new DB())
            {
                try
                {
                    Staff entity = db.Staff
                        .Where(x => x.StaffId == toSave.StaffId)
                        .FirstOrDefault();

                    if(entity == null)
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
                        entity.CreatedBy = tempEmail;
                        db.Staff.Add(entity);
                    }

                    entity.StaffTypeId = toSave.StaffTypeId;
                    entity.TaskId = toSave.TaskId;
                    entity.UserId = toSave.UserId;
                    entity.IsActive = toSave.IsActive;
                    entity.UpdatedDate = DateTime.Now;
                    entity.UpdatedBy = tempEmail;
                    db.SaveChanges();

                    return entity.StaffId;
                }
                catch(Exception ex)
                {
                    // ex.Publish();
                    // return null;
                    throw new Exception("Exception thrown in Staffs.Save");
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
                    // ex.Publish();
                    throw new Exception("Exception thrown in Staffs.Delete");
                }
            }
        }
    }
}
