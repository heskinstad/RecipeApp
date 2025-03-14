import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import './style.css';

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
        <div className="recipeUpperDiv">
            <div recipeTitle className="recipeTitle">
                <h1>{recipe.name}</h1>
                <br />
                <hr />
            </div>
            <img src={recipe.imagePath} className="recipeImageLarge" />
        </div>
    )
};

export default Recipe;