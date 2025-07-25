// Code-behind for EntityTypes.razor
using AppOverview.Model;
using Microsoft.AspNetCore.Components;

namespace AppOverview.Components.Pages
{
    public partial class EntityTypes : ComponentBase
    {
        private List<EntityTypeDTO>? entityTypes;
        private EntityTypeDTO editType = new EntityTypeDTO();
        private bool showForm = false;
        private bool isEdit = false;

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(300); // Simulate async loading
            var data = await DataProvider.GetEntityTypesAsync();
            entityTypes = data.ToList();
        }

        private void ShowNewForm()
        {
            editType = new EntityTypeDTO();
            showForm = true;
            isEdit = false;
        }

        private void ShowEditForm(EntityTypeDTO type)
        {
            editType = type;
            showForm = true;
            isEdit = true;
        }

        private void CancelEdit()
        {
            showForm = false;
        }

        private async Task OnSubmitType()
        {
            if (entityTypes == null) return;
            if (isEdit)
            {
                var idx = entityTypes.FindIndex(t => t.Id == editType.Id);
                if (idx >= 0)
                {
                    entityTypes[idx] = editType;
                    await DataProvider.UpdateEntityTypeAsync(editType);
                }
            }
            else
            {
                var newId = entityTypes.Count > 0 ? entityTypes.Max(t => t.Id) + 1 : 1;
                editType.Id = newId;
                entityTypes.Add(editType);
                await DataProvider.AddEntityTypeAsync(editType);
            }
            showForm = false;
            StateHasChanged();
        }
    }
}
