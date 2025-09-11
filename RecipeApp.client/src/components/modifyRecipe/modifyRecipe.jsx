import { useEffect, useState } from 'react';
import './modifyRecipe.css';
import AddIngredient from '../addIngredient/addIngredient';
import Popup from '../popup/popup';
import RichTextBox from '../richTextBox/richTextBox';

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



    useEffect(() => {
        fetch(categoriesUrl)
            .then((res) => res.json())
            .then(setCategories)
            .catch(console.error);
    }, []);

    if (categories.length === 0) {
        return;
    }

    const openPopup = () => {
        setIsPopupOpen(true);
    }

    const closePopup = () => {
        setIsPopupOpen(false);
    }

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
                        />
                    </label>
                </div>

                <div className="modifyRecipe_desc">
                    <label>
                        <h3>Description:</h3>
                        <br />
                        {/* <textarea
                            name="description"
                            onChange={handleChange}
                            value={formData.description}
                        /> */}
                        <RichTextBox />
                    </label>
                </div>

                <div className="modifyRecipe_category">
                    <label>
                        <h3>Category:</h3>
                        <br />
                        <select
                            name="categoryId"
                            onChange={handleChange}
                            value={formData.categoryId?.toString() || ""}
                        >
                            <option value="">-- Select Category --</option>
                            {categories.map((category) => (
                                <option value={category.id.toString()} key={category.id}>
                                    {category.name}
                                </option>
                            ))}
                        </select>
                        <div>
                            <br />
                            <p>Or add a new category:</p>
                            <input
                                type="text"
                                name="newCategory"
                                onChange={handleChange}
                                value={formData.newCategory || ""}
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
                    <input type="submit" value={isEditMode ? "Update" : "Create"} />
                </div>
            </form>

            {isEditMode && (
            <div className="modifyRecipe_delete">
                <button onClick={openPopup}>Delete recipe</button>
                <Popup
                message="Are you sure you want to delete this recipe?"
                handleAction={handleDelete}
                onClose={closePopup}
                isOpen={isPopupOpen}
                />
            </div>
            )}
        </>
    );
}

export default ModifyRecipe;
