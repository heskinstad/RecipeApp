import { useEffect, useState } from 'react';
import './addRecipe.css';
import { useNavigate } from 'react-router-dom';

function AddRecipe() {
    const navigate = useNavigate();
    const [data, setData] = useState([]);
    const [formData, setFormData] = useState({
        name: "",
        summary: "",
        description: "",
        imagePath: "",
    });

    const url = "https://localhost:63516/recipe"

    const postRecipe = () => {
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(formData),
        })
        .then((res) => res.json())
        .then(() => fetchData())
        .catch((err) => console.log(err))
    }

    const handleChange = (event) => {
        setFormData({ ...formData, [event.target.name]: event.target.value });
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        postRecipe();
        navigate(`/`);
    }

    return (
        <form className="addRecipe_form" onSubmit={handleSubmit}>
            <div className="addRecipe_header">
                <h1>Add a New Recipe</h1>
            </div>
            <div className="addRecipe_name">
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
                    <textarea
                    type="text"
                    name="summary"
                    onChange={handleChange}
                    value={formData.summary}
                    />
                </label>
            </div>
            <div className="addRecipe_desc">
                <label>
                    <h3>Description:</h3>
                    <br />
                    <textarea
                    type="text"
                    name="description"
                    onChange={handleChange}
                    value={formData.description}
                    />
                </label>
            </div>
            <div className="addRecipe_uploadImg">
                <label>
                    <h3>Upload image:</h3>
                    <br />
                    <input
                    type="text" //type="file"
                    name="imagePath"
                    onChange={handleChange}
                    value={formData.imagePath}
                    />
                </label>
            </div>
            <div className="addRecipe_submit">
                <input
                type="submit"
                value="Create"
                />
            </div>
        </form>
    )
}

export default AddRecipe;