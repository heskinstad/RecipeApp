import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import IngredientListItem from '../../components/ingredientListItem/ingredientListItem';
import StarRatingDisplay from '../../components/starRatingDisplay/starRatingDisplay';
import './style.css';
import Collapsible from '../../components/collapsible/collapsible';

function Recipe() {
    const { id } = useParams();

    const recipesUrl = `https://localhost:63516/recipe/${id}`;
    const [recipe, setRecipe] = useState([]);

    const ingredientsUrl = `https://localhost:63516/recipe/${id}/ingredients`;
    const [ingredients, setIngredients] = useState([]);

    const recipeRatingUrl = `https://localhost:63516/recipe/${id}/averageRating`;
    const [recipeRating, setRecipeRating] = useState([]);

    const recipeRatingCountUrl = `https://localhost:63516/recipe/${id}/ratingsCount`;
    const [recipeRatingCount, setRecipeRatingCount] = useState([]);

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

    const fetchRecipeRating = () => {
        fetch(recipeRatingUrl)
        .then((res) => {
            return res.json();
        })
        .then((jsonData) => {
            setRecipeRating(jsonData);
        })
    };

    const fetchRecipeRatingCount = () => {
        fetch(recipeRatingCountUrl)
        .then((res) => {
            return res.json();
        })
        .then((jsonData) => {
            setRecipeRatingCount(jsonData);
        })
    };

    useEffect(() => {
        fetchRecipes();
        fetchIngredients();
        fetchRecipeRating();
        fetchRecipeRatingCount();
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
            <div className="recipeRating">
                <p>
                    Give this recipe a rating!
                </p>
                <StarRatingDisplay name={`rating-${recipe.id}`} recipeRatingCount={recipeRatingCount} recipeRating={recipeRating} />
            </div>
            <div className="recipeComments">
                <Collapsible label="Comments">
                    <p>tete</p>
                </Collapsible>
            </div>
        </div>
    )
};

export default Recipe;