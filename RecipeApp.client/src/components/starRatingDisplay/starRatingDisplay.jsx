import React, { useEffect, useState } from "react";
import './style.css';

function StarRatingDisplay({recipeRatingCount, recipeRating, name}) {
    const [selectedRating, setSelectedRating] = useState(recipeRating);

    useEffect(() => {
        setSelectedRating(Math.round(recipeRating));
      }, [recipeRating]);

    return (
        <div className="upperDiv">
            <div className="rate">
                <input
                type="radio"
                id={`${name}-star5`}
                name={name} // Use the passed `name` prop here
                value="5"
                checked={selectedRating === 5}
                onChange={() => setSelectedRating(5)}
                />
                <label htmlFor={`${name}-star5`} title="text">
                5 stars
                </label>

                <input
                type="radio"
                id={`${name}-star4`}
                name={name} // Use the passed `name` prop here
                value="4"
                checked={selectedRating === 4}
                onChange={() => setSelectedRating(4)}
                />
                <label htmlFor={`${name}-star4`} title="text">
                4 stars
                </label>

                <input
                type="radio"
                id={`${name}-star3`}
                name={name} // Use the passed `name` prop here
                value="3"
                checked={selectedRating === 3}
                onChange={() => setSelectedRating(3)}
                />
                <label htmlFor={`${name}-star3`} title="text">
                3 stars
                </label>

                <input
                type="radio"
                id={`${name}-star2`}
                name={name} // Use the passed `name` prop here
                value="2"
                checked={selectedRating === 2}
                onChange={() => setSelectedRating(2)}
                />
                <label htmlFor={`${name}-star2`} title="text">
                2 stars
                </label>

                <input
                type="radio"
                id={`${name}-star1`}
                name={name} // Use the passed `name` prop here
                value="1"
                checked={selectedRating === 1}
                onChange={() => setSelectedRating(1)}
                />
                <label htmlFor={`${name}-star1`} title="text">
                1 star
                </label>
            </div>
            <br />
            <p>({recipeRating} from {recipeRatingCount} ratings)</p>
        </div>
    )
};

export default StarRatingDisplay;