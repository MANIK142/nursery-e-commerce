using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Catalog.Application.Dtos;

public record ImageUploadDto(Guid? PlantId, Guid? VariantId, IFormFile File, string AltName, bool IsPrimary);

