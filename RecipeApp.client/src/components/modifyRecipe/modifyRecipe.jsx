import { useEffect, useState } from 'react';
import { Node } from 'slate';
import Select from 'react-select';
import './modifyRecipe.css';
import AddIngredient from '../addIngredient/addIngredient';
import Popup from '../popup/popup';
import RichTextBox from '../richTextBox/richTextBox';

const serialize = value => {
  return value.map(n => Node.string(n)).join('\n');
};

const deserialize = string => {
  return string.split('\n').map(line => {
    return { children: [{ text: line }] };
  });
};

function ModifyRecipe({
    formData,
    handleChange,
    handleSubmit,
    handleDelete,
    isEditMode = false,
}) {
    const [categories, setCategories] = useState([]);
    const categoriesUrl = "https://localhost:63516/category";
    const [isPopupOpen, setIsPopupOpen] = useState(false);

    const formattedOptions = categories.map((category) => ({
        value: category.id.toString(),
        label: category.name
    }));

    const currentSelectValue = formattedOptions.find(
        (option) => option.value === formData.categoryId?.toString()
    ) || null;

    const handleSelectChange = (selectedOption) => {
        handleChange({
            target: {
                name: 'categoryId',
                value: selectedOption ? selectedOption.value : ''
            }
        });
    };

    useEffect(() => {
        fetch(categoriesUrl)
            .then((res) => res.json())
            .then(setCategories)
            .catch(console.error);
    }, []);

    if (categories.length === 0) {
        return null; // Fixed: Functional components should return null instead of empty undefined
    }

    const openPopup = () => {
        setIsPopupOpen(true);
    };

    const closePopup = () => {
        setIsPopupOpen(false);
    };

    // --- FORM VALIDATION LOGIC ---
    // Verifies required string fields are not empty or just whitespace
    const hasName = formData.name?.trim();
    const hasSummary = formData.summary?.trim();
    const hasImagePath = formData.imagePath?.trim();
    
    // Verifies a category selection exists OR a custom new category string is typed
    const hasCategory = formData.categoryId?.toString().trim() || formData.newCategory?.trim();

    // Verifies Slate Rich Text editor content is not empty or just a blank HTML tag
    const hasDescription = formData.description?.trim() && formData.description !== "<p></p>";

    const isFormValid = hasName && hasSummary && hasImagePath && hasCategory && hasDescription;

    return (
        <>
            <form className="modifyRecipe_form" onSubmit={handleSubmit}>
                <div className="modifyRecipe_header">
                    <h1>{isEditMode ? "Edit Recipe" : "Add a New Recipe"}</h1>
                </div>

                <div className="modifyRecipe_name">
                    <label>
                        <h3>Recipe Name:</h3>
                        <br />
                        <input
                            type="text"
                            name="name"
                            onChange={handleChange}
                            value={formData.name}
                            placeholder="Enter recipe name..."
                        />
                    </label>
                </div>

                <div className="modifyRecipe_ingredients">
                    <label>
                        <AddIngredient />
                    </label>
                </div>

                <div className="modifyRecipe_summary">
                    <label>
                        <h3>Summary:</h3>
                        <br />
                        <textarea
                            name="summary"
                            onChange={handleChange}
                            value={formData.summary}
                            placeholder="Write a summary..."
                        />
                    </label>
                </div>

                <div className="modifyRecipe_desc">
                    <label>
                        <h3>Description:</h3>
                        <br />
                        <RichTextBox 
                            name="description"
                            onChange={handleChange}
                            value={formData.description}
                        />
                    </label>
                </div>

                <div className="modifyRecipe_category">
                    <label>
                        <h3>Category:</h3>
                        <br />
                        <div className="select-wrapper">
                            <Select
                                name="categoryId"
                                options={formattedOptions}
                                value={currentSelectValue}
                                onChange={handleSelectChange}
                                placeholder="-- Select Category --"
                                isClearable // Allows users to clear selection back to empty placeholder
                            />
                        </div>
                    </label>
                </div>

                <div className="modifyRecipe_uploadImg">
                    <label>
                        <h3>Upload image:</h3>
                        <br />
                        <input
                            type="text" // Switch to file in the future
                            name="imagePath"
                            onChange={handleChange}
                            value={formData.imagePath}
                        />
                    </label>
                </div>

                <div className="modifyRecipe_submit">
                    {/* FIXED: Added 'disabled' property, removed window.history.back() from click */}
                    <button type="submit" disabled={!isFormValid}>
                        {isEditMode ? "Update" : "Create"}
                    </button>
                    <button type="button" className="modifyRecipe_cancel" onClick={() => window.history.back()}>
                        Cancel
                    </button>
                </div>
                
                {isEditMode && (
                    <div className="modifyRecipe_delete">
                        {/* FIXED: Changed type to "button" so clicking this doesn't accidentally trigger form submit */}
                        <button type="button" onClick={openPopup}>Delete recipe</button>
                        <Popup
                            message="Are you sure you want to delete this recipe?"
                            handleAction={handleDelete}
                            onClose={closePopup}
                            isOpen={isPopupOpen}
                        />
                    </div>
                )}
            </form>
        </>
    );
}

export default ModifyRecipe;
