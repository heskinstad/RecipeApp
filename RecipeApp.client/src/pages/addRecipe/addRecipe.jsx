import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import ModifyRecipe from '../../components/modifyRecipe/modifyRecipe';



function AddRecipe() {
    const navigate = useNavigate();

    const [formData, setFormData] = useState({
        name: "",
        summary: "",
        description: "<p></p>",
        categoryId: "",
        imagePath: "",
        uploaderId: "019e2028-a994-7750-96d3-78323ac84807",
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
