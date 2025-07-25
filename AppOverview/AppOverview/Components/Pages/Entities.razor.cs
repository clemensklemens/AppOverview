// Code-behind for Entities.razor
using AppOverview.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AppOverview.Components.Pages
{
    public partial class Entities : ComponentBase
    {
        private List<EntityDTO>? entities;
        private List<DepartmentDTO>? departments;
        private List<TechnologyDTO>? technologies;
        private List<EntityTypeDTO>? entityTypes;
        private EntityDTO editEntity = new EntityDTO();
        private bool showForm = false;
        private bool isEdit = false;

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(300); // Simulate async loading
            var enityData = await DataProvider.GetEntitiesAsync();
            entities = enityData.ToList();
            var departmentData = await DataProvider.GetDepartmentsAsync();
            departments = departmentData.ToList();
            var technologyData = await DataProvider.GetTechnologiesAsync();
            technologies = technologyData.ToList();
            var entityTypeData = await DataProvider.GetEntityTypesAsync();
            entityTypes = entityTypeData.ToList();
        }

        private void ShowNewForm()
        {
            editEntity = new EntityDTO();
            showForm = true;
            isEdit = false;
        }

        private void ShowEditForm(EntityDTO entity)
        {
            editEntity = entity;
            showForm = true;
            isEdit = true;
        }

        private void CancelEdit()
        {
            showForm = false;
        }

        private async Task OnSubmitEntity()
        {
            if (entities == null)
            {
                return;
            }

            editEntity = FindNestedValues(editEntity);
            if (isEdit)
            {
                var idx = entities.FindIndex(e => e.Id == editEntity.Id);
                if (idx >= 0)
                {
                    entities[idx] = editEntity;
                    await DataProvider.UpdateEntityAsync(editEntity);
                }
            }
            else
            {
                var newId = entities.Count > 0 ? entities.Max(e => e.Id) + 1 : 1;
                editEntity.Id = newId;
                entities.Add(editEntity);
                await DataProvider.AddEntityAsync(editEntity);
            }
            showForm = false;
            StateHasChanged();
        }

        private EntityDTO FindNestedValues(EntityDTO dto)
        {
            dto.Department = departments?.FirstOrDefault(d => d.Id == dto.DepartmentId)?.Name ?? string.Empty;
            dto.Technology = technologies?.FirstOrDefault(t => t.Id == dto.TechnologyId)?.Name ?? string.Empty;
            dto.Type = entityTypes?.FirstOrDefault(et => et.Id == dto.TypeId)?.Name ?? string.Empty;
            dto.ColorHex = entityTypes?.FirstOrDefault(et => et.Id == dto.TypeId)?.ColorHex ?? "#FFFFFF"; // Default color if not found
            return dto;
        }
    }
}
