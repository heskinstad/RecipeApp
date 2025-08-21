import './addRecipe.css';

function AddRecipe() {
    return (
        <form className="addRecipe_form">
            <div className="addRecipe_header">
                <h1>Add a New Recipe</h1>
            </div>
            <div className="addRecipe_name">
                <label>
                    <h3>Recipe Name:</h3>
                    <br />
                    <input type="text" name="name" />
                </label>
            </div>
            <div className="addRecipe_ingredients">
                <label>
                    <h3>Ingredients:</h3>
                    <br />
                    <textarea name="ingredients" />
                </label>
            </div>
            <div className="addRecipe_summary">
                <label>
                    <h3>Summary:</h3>
                    <br />
                    <textarea type="text" name="summary" />
                </label>
            </div>
            <div className="addRecipe_desc">
                <label>
                    <h3>Description:</h3>
                    <br />
                    <textarea type="text" name="description" />
                </label>
            </div>
            <div className="addRecipe_uploadImg">
                <label>
                    <h3>Upload image:</h3>
                    <br />
                    <input type="file" name="image" />
                </label>
            </div>
            <div className="addRecipe_submit">
                <button type="submit">Add Recipe</button>
            </div>
        </form>
    )
}

export default AddRecipe;