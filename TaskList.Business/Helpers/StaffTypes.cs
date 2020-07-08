using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.Core.DTOs;
using TaskList.Data.Model;

namespace TaskList.Business.Helpers
{
    public static class StaffTypes
    {
        public static StaffTypeDTO GetByID(Guid id)
        {
            using (var db = new DB())
            {
                var dto = db.StaffType
                    .Where(x => x.StaffTypeId == id)
                    .Select(x => new StaffTypeDTO()
                    {
                        StaffTypeId = x.StaffTypeId,
                        Name = x.Name,
                        AllowMultiple = x.AllowMultiple,
                        IsSupervisor = x.IsSupervisor,
                        DisplayOrder = x.DisplayOrder,
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

        public static List<StaffTypeDTO> GetAll(bool activeOnly = false)
        {
            using (var db = new DB())
            {
                var dtos = db.StaffType
                    .Select(x => new StaffTypeDTO()
                    {
                        StaffTypeId = x.StaffTypeId,
                        Name = x.Name,
                        AllowMultiple = x.AllowMultiple,
                        IsSupervisor = x.IsSupervisor,
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

        public static Guid? Save(StaffTypeDTO toSave)
        {
            // TODO: Should be current user
            var tempEmail = "hscheffert@qci.com";

            using (var db = new DB())
            {
                try
                {
                    StaffType entity = db.StaffType
                        .Where(x => x.StaffTypeId == toSave.StaffTypeId)
                        .FirstOrDefault();

                    if(entity == null)
                    {
                        if(toSave.StaffTypeId == Guid.Empty)
                        {
                            entity = new StaffType();
                            entity.StaffTypeId = Guid.NewGuid();
                        }
                        else
                        {
                            entity = new StaffType();
                            entity.StaffTypeId = toSave.StaffTypeId;
                        }

                        entity.CreatedDate = DateTime.Now;
                        entity.CreatedBy = tempEmail;
                        db.StaffType.Add(entity);
                    }

                    entity.Name = toSave.Name;
                    entity.AllowMultiple = toSave.AllowMultiple;
                    entity.IsSupervisor = toSave.IsSupervisor;
                    entity.DisplayOrder = toSave.DisplayOrder;
                    entity.IsActive = toSave.IsActive;
                    entity.UpdatedDate = DateTime.Now;
                    entity.UpdatedBy = tempEmail;
                    db.SaveChanges();

                    return entity.StaffTypeId;
                }
                catch(Exception ex)
                {
                    // ex.Publish();
                    // return null;
                    throw new Exception("Exception thrown in StaffTypes.Save");
                }
            }
        }

        public static void Toggle(Guid id)
        {
            using(var db = new DB())
            {
                try
                {
                    StaffType entity = db.StaffType
                        .Where(x => x.StaffTypeId == id)
                        .FirstOrDefault();

                    entity.IsActive = !entity.IsActive;
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    // ex.Publish();
                    throw new Exception("Exception thrown in StaffTypes.Toggle");               
                }                
            }
        }

        public static Guid GetIDByName(string name)
        {
            using (var db = new DB())
            {
                var id = db.StaffType
                    .Where(x => x.Name == name)
                    .Select(x => x.StaffTypeId)
                    .FirstOrDefault();

                return id;
            }
        }
    }
}
