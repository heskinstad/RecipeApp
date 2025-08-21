import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import IngredientListItem from '../../components/ingredientListItem/ingredientListItem';
import StarRatingDisplay from '../../components/starRatingDisplay/starRatingDisplay';
import CommentBlock from '../../components/commentBlock/commentBlock';
import './recipe.css';
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

    const commentsUrl = `https://localhost:63516/recipe/${id}/comments`;
    const [comments, setComments] = useState([]);

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

    const fetchComments = () => {
        fetch(commentsUrl)
        .then((res) => {
            return res.json();
        })
        .then((jsonData) => {
            setComments(jsonData);
        })
    };

    useEffect(() => {
        fetchRecipes();
        fetchIngredients();
        fetchRecipeRating();
        fetchRecipeRatingCount();
        fetchComments();
    }, []);

    const groupedIngredients = ingredients.reduce((groups, ingredient) => {
        const section = ingredient.section || 'Uncategorized';
        if (!groups[section]) {
            groups[section] = [];
        }
        groups[section].push(ingredient);
        return groups;
    }, {});

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
                <h2>Ingredients</h2>
                {Object.entries(groupedIngredients).map(([section, items]) => (
                    <div key={section}>
                        <h4>{section}</h4>
                        {items.map((ingredient, index) => (
                            <IngredientListItem ingredient={ingredient} key={index} />
                        ))}
                    </div>
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
                    {comments.map((comment) => (
                        <CommentBlock comment={comment} key={comment.id} />
                    ))}
                </Collapsible>
            </div>
        </div>
    )
};

export default Recipe;