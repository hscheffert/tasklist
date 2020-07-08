using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.Core.DTOs;
using TaskList.Data.Model;

namespace TaskList.Business.Helpers
{
    public static class Frequencies
    {
        public static FrequencyDTO GetByID(Guid id)
        {
            using (var db = new DB())
            {
                var dto = db.Frequency
                    .Where(x => x.FrequencyId == id)
                    .Select(x => new FrequencyDTO()
                    {
                        FrequencyId = x.FrequencyId,
                        Name = x.Name,
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

        public static List<FrequencyDTO> GetAll(bool activeOnly = false)
        {
            using (var db = new DB())
            {
                var dtos = db.Frequency
                    .Select(x => new FrequencyDTO()
                    {
                        FrequencyId = x.FrequencyId,
                        Name = x.Name,
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

        public static Guid? Save(FrequencyDTO toSave)
        {
            // TODO: Should be current user
            var tempEmail = "hscheffert@qci.com";

            using (var db = new DB())
            {
                try
                {
                    Frequency entity = db.Frequency
                        .Where(x => x.FrequencyId == toSave.FrequencyId)
                        .FirstOrDefault();

                    if(entity == null)
                    {
                        if(toSave.FrequencyId == Guid.Empty)
                        {
                            entity = new Frequency();
                            entity.FrequencyId = Guid.NewGuid();
                        }
                        else
                        {
                            entity = new Frequency();
                            entity.FrequencyId = toSave.FrequencyId;
                        }

                        entity.CreatedDate = DateTime.Now;
                        entity.CreatedBy = tempEmail;
                        db.Frequency.Add(entity);
                    }

                    entity.Name = toSave.Name;
                    entity.DisplayOrder = toSave.DisplayOrder;
                    entity.IsActive = toSave.IsActive;
                    entity.UpdatedDate = DateTime.Now;
                    entity.UpdatedBy = tempEmail;
                    db.SaveChanges();

                    return entity.FrequencyId;
                }
                catch(Exception ex)
                {
                    throw new Exception("Exception thrown in Frequencies.Save");
                }
            }
        }

        public static void Toggle(Guid id)
        {
            using(var db = new DB())
            {
                try
                {
                    Frequency entity = db.Frequency
                        .Where(x => x.FrequencyId == id)
                        .FirstOrDefault();

                    entity.IsActive = !entity.IsActive;
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    throw new Exception("Exception thrown in Frequencies.Toggle");               
                }                
            }
        }
    }
}
