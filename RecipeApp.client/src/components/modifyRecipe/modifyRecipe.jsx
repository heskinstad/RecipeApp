import { useEffect, useState } from 'react';
import './modifyRecipe.css';

function ModifyRecipe({
    formData,
    handleChange,
    handleSubmit,
    isEditMode = false,
}) {
    const [categories, setCategories] = useState([]);
    const categoriesUrl = "https://localhost:63516/category";

    useEffect(() => {
        fetch(categoriesUrl)
            .then((res) => res.json())
            .then(setCategories)
            .catch(console.error);
    }, []);

    // 🛑 Don't render until categories are loaded
    if (categories.length === 0) {
        return <p>Loading categories...</p>;
    }

    const handleChanges = (e) => {
        console.log("Changed:", e.target.name, e.target.value); // 🐞 Debug
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };


    return (
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
                    <h3>Ingredients:</h3>
                    <br />
                    <textarea
                        name="ingredients"
                        onChange={handleChange}
                        value={formData.ingredients || ""}
                    />
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
                    <textarea
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
    );
}

export default ModifyRecipe;
