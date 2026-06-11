import { useEffect, useState } from "react";
import './ratingBlock.css';

// Added the 'interactive' prop with a default value of false
function RatingBlock({ recipeRatingCount, recipeRating, interactive = false }) {
  const [selectedRating, setSelectedRating] = useState(
    Number(recipeRating).toFixed(1)
  );
  const [isEditing, setIsEditing] = useState(false);

  useEffect(() => {
    if (recipeRating !== undefined && recipeRating !== null) {
      setSelectedRating(Number(recipeRating).toFixed(1));
    }
  }, [recipeRating]);

  const handleVoteSubmit = (userVote) => {
    console.log(`User voted: ${userVote}`);
    setIsEditing(false); 
  };

  // Scenario A: Render Interactive Button with Popups
  if (interactive) {
    return (
      <div className="ratingBlock_container">
        <button 
          className="ratingBlock_block ratingBlock_clickable" 
          onClick={(e) => {
            e.stopPropagation();
            setIsEditing(true);
          }}
        >
          {selectedRating}
        </button>
        <span className="ratingBlock_labelText">
          from {recipeRatingCount} votes
        </span>

        {isEditing && (
          <div 
            className="ratingBlock_overlay"
            onClick={(e) => {
              e.stopPropagation();
              setIsEditing(false);
            }}
          >
            <div className="ratingBlock_modal" onClick={(e) => e.stopPropagation()}>
              <h3>Rate this recipe</h3>
              <div className="ratingBlock_grid">
                {[1, 2, 3, 4, 5, 6, 7, 8, 9, 10].map((num) => (
                  <button
                    key={num}
                    className="ratingBlock_numOption"
                    onClick={() => handleVoteSubmit(num)}
                  >
                    {num}
                  </button>
                ))}
              </div>
              <button 
                className="ratingBlock_cancelBtn"
                onClick={() => setIsEditing(false)}
              >
                Cancel
              </button>
            </div>
          </div>
        )}
      </div>
    );
  }

  // Scenario B: Render Static Non-Interactive Display (For RecipeItem lists)
  return (
    <div className="ratingBlock_container">
      <div className="ratingBlock_block ratingBlock_static">
        {selectedRating}
      </div>
      <span className="ratingBlock_labelText">
        from {recipeRatingCount} votes
      </span>
    </div>
  );
}

export default RatingBlock;
