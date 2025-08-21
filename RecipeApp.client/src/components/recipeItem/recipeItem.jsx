import './recipeItem.css';
import { Link } from "react-router-dom";
import { useEffect, useState } from 'react'
import StarRatingDisplay from '../starRatingDisplay/starRatingDisplay';

function RecipeItem({recipe}) {
    const recipeRatingUrl = `https://localhost:63516/recipe/${recipe.id}/averageRating`;
    const [recipeRating, setRecipeRating] = useState([]);

    const recipeRatingCountUrl = `https://localhost:63516/recipe/${recipe.id}/ratingsCount`;
    const [recipeRatingCount, setRecipeRatingCount] = useState([]);

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
        fetchRecipeRating();
        fetchRecipeRatingCount();
    }, [recipe]);

    return (
        <Link to={`/recipe/${recipe.id}`.toLowerCase()}>
            <hr />
            <div className="recipeItem_link">
                <h2 className="recipeItem_title">{recipe.name}</h2>
                <p className="recipeItem_desc">{recipe.summary}</p>
                <img src={recipe.imagePath} className="recipeItem_recipeImage" />
                <div className="recipeItem_uploader">
                    <br />
                    <h4 >Uploader: {recipe.uploaderName}</h4>
                </div>
                <div className="recipeItem_rating">
                    <StarRatingDisplay name={`rating-${recipe.id}`} recipeRatingCount={recipeRatingCount} recipeRating={recipeRating} />
                </div>
            </div>
        </Link>
    )
};

export default RecipeItem;