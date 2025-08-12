import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import IngredientListItem from '../../components/ingredientListItem/ingredientListItem';
import './style.css';

function Recipe() {
    const { id } = useParams();

    const recipesUrl = `https://localhost:63516/recipe/${id}`;
    const [recipe, setRecipe] = useState([]);

    const ingredientsUrl = `https://localhost:63516/recipe/${id}/ingredients`;
    const [ingredients, setIngredients] = useState([]);

    const fetchRecipes = () => {
        fetch(recipesUrl)
        .then((res) => {
          return res.json();
        })
        .then((jsonData) => {
          setRecipe(jsonData);
        })
    };

    const fetchIngredients = () => {
        fetch(ingredientsUrl)
        .then((res) => {
            return res.json();
        })
        .then((jsonData) => {
            setIngredients(jsonData);
        })
    };

    useEffect(() => {
        fetchRecipes();
        fetchIngredients();
    }, []);

    return (
        <div className="recipeUpperDiv">
            <div className="recipeTitle">
                <h1>{recipe.name}</h1>
                <br />
                <hr />
            </div>
            <div className="recipeSummary">
                <p>
                    {recipe.summary}
                </p>
                <br />
            </div>
            <img src={recipe.imagePath} className="recipeImageLarge" />
            <div className="recipeIngredientsBox">
                {ingredients.map((ingredient, index) => (
                    <IngredientListItem ingredient={ingredient} key={index} />
                ))}
            </div>
            <div className="recipeDescription">
                {recipe.description}
            </div>
        </div>
    )
};

export default Recipe;