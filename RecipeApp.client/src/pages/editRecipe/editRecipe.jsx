import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import ModifyRecipe from '../../components/modifyRecipe/modifyRecipe';

function EditRecipe() {
    const { id } = useParams();
    const navigate = useNavigate();

    const [formData, setFormData] = useState(null);

    const recipeUrl = `https://localhost:63516/recipe/${id}`;

    useEffect(() => {
        fetch(recipeUrl)
            .then((res) => res.json())
            .then((data) => setFormData(data))
            .catch(console.error);
    }, [id]);

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const deleteRecipe = () => {
        fetch(recipeUrl, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(formData),
        })
        .catch((err) => console.log(err))
    }

    const handleDelete = () => {
        deleteRecipe();
        navigate('/');
    }

    const handleSubmit = (e) => {
        e.preventDefault();
            const dataToSend = {
            name: formData.name,
            summary: formData.summary,
            description: formData.description,
            imagePath: formData.imagePath,
            categoryId: formData.categoryId,
            uploaderId: formData.uploaderId,
        };

        fetch(recipeUrl, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(dataToSend),
        })
            .then(() => {
                navigate('/');
            })
            .catch(console.error);
    };

    if (!formData) return <div>Loading...</div>;

    return (
        <ModifyRecipe
            formData={formData}
            handleChange={handleChange}
            handleSubmit={handleSubmit}
            handleDelete={handleDelete}
            isEditMode={true}
        />
    );
}

export default EditRecipe;
