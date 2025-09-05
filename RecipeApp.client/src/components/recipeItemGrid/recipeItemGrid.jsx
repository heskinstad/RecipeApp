import './recipeItemGrid.css';
import { Link } from "react-router-dom";
import { useEffect, useState } from 'react'
import StarRatingDisplay from '../starRatingDisplay/starRatingDisplay';
import missing_image from "../../resources/buttons/missing_image.png";

function RecipeItemGrid({recipe}) {
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
            <div className="recipeItemGrid_link">
                <h2 className="recipeItemGrid_title">{recipe.name}</h2>
                <img
                src={recipe.imagePath}
                className="recipeItemGrid_recipeImage"
                onError={(e) => {
                                e.target.onerror = null;
                                e.target.src = missing_image;
                            }}
                />
                <div className="recipeItemGrid_rating">
                    <StarRatingDisplay name={`rating-${recipe.id}`} recipeRatingCount={recipeRatingCount} recipeRating={recipeRating} />
                </div>
            </div>
        </Link>
    )
};

export default RecipeItemGrid;