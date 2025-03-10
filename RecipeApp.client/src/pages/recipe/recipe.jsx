import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

function Recipe() {
    const { id } = useParams();

    const recipesUrl = `https://localhost:63516/recipe/${id}`;
    const [recipe, setRecipe] = useState([]);

    const fetchRecipes = () => {
        fetch(recipesUrl)
        .then((res) => {
          return res.json();
        })
        .then((jsonData) => {
          setRecipe(jsonData);
        })
    };

    useEffect(() => {
        fetchRecipes();
    }, []);

    return (
        <>
            <h1>{recipe.name}</h1>
        </>
    )
};

export default Recipe;