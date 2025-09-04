import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import ModifyRecipe from '../../components/modifyRecipe/modifyRecipe';

function AddRecipe() {
    const navigate = useNavigate();

    const [formData, setFormData] = useState({
        name: "",
        summary: "",
        description: "",
        categoryId: "",
        imagePath: "",
        uploaderId: "0198a3ae-a9d7-7dfe-9031-12e95619f54f",
    });

    const recipeUrl = "https://localhost:63516/recipe";
    
    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        fetch(recipeUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(formData),
        })
            .then(() => {
                navigate("/");
            })
            .catch(console.error);
    };

    return (
        <ModifyRecipe
            formData={formData}
            handleChange={handleChange}
            handleSubmit={handleSubmit}
            isEditMode={false}
        />
    );
}

export default AddRecipe;
