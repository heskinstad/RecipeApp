import './style.css';
import { Link} from "react-router-dom";
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
        <div className="link">
            <Link to={`/recipe/${recipe.id}`.toLowerCase()}>
                <hr />
                <div className="recipeItemGrid">
                    <h2 className="title">{recipe.name}</h2>
                    <p className="desc">{recipe.summary}</p>
                    <img src={recipe.imagePath} className="recipeImage" />
                    <div className="uploader">
                        <br />
                        <h4 >Uploader: {recipe.uploaderName}</h4>
                    </div>
                    <div className="rating">
                        <StarRatingDisplay name={`rating-${recipe.id}`} recipeRatingCount={recipeRatingCount} recipeRating={recipeRating} />
                    </div>
                </div>
            </Link>
        </div>
    )
};

export default RecipeItem;