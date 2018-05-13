using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ConfigurationReader.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;

namespace ConfigurationReader.Dashboard.Models {
    public class CreateViewModel : IValidatableObject {
        [Required]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string ApplicationName { get; set; }

        [Required]
        public string Value { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            var result = new List<ValidationResult>();

            var storageProvider = validationContext.GetRequiredService<IStorageProvider<ObjectId>>();
            var conflictedValue = storageProvider.Get(ApplicationName, Name).GetAwaiter().GetResult();

            if (conflictedValue != null) {
                result.Add(new ValidationResult(
                    $"There is already a record with '{Name}' name.",
                    new[] { nameof(Name) }));
            }

            return result;
        }
    }
}