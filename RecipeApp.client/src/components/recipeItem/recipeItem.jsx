import './recipeItem.css';
import { Link } from "react-router-dom";
import { useEffect, useState } from 'react'
import StarRatingDisplay from '../starRatingDisplay/starRatingDisplay';
import missing_image from "../../resources/buttons/missing_image.png";
import RatingBlock from '../ratingBlock/ratingBlock';

function RecipeItem({recipe}) {

    return (
        <Link to={`/recipe/${recipe.id}`.toLowerCase()}>
            <div className="recipeItem_link">
                <h2 className="recipeItem_title">{recipe.name}</h2>
                <p className="recipeItem_desc">{recipe.summary}</p>
                <img
                src={recipe.imagePath}
                className="recipeItem_recipeImage"
                onError={(e) => {
                                e.target.onerror = null;
                                e.target.src = missing_image;
                            }}
                />
                <div className="recipeItem_uploader">
                    <br />
                    <h4>Uploader: {recipe.uploaderName}</h4>
                </div>
                <div className="recipeItem_rating">
                    <RatingBlock recipeRating={recipe.avgRating} recipeId={recipe.id} />
                </div>
                <div className="recipeItem_visits">
                    <br />
                    <h4> {recipe.visits} visits</h4>
                </div>
            </div>
        </Link>
    )
};

export default RecipeItem;