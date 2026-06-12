import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import IngredientListItem from '../../components/ingredientListItem/ingredientListItem';
import RatingBlock from '../../components/ratingBlock/ratingBlock';
import CommentBlock from '../../components/commentBlock/commentBlock';
import './recipe.css';
import Collapsible from '../../components/collapsible/collapsible';
import missing_image from "../../resources/buttons/missing_image.png";
import { Link } from "react-router-dom";
import AddComment from '../../components/addComment/addComment';

function Recipe() {
    const { id } = useParams();

    const recipesUrl = `https://localhost:63516/recipe/${id}`;
    const [recipe, setRecipe] = useState([]);

    const ingredientsUrl = `https://localhost:63516/recipe/${id}/ingredients`;
    const [ingredients, setIngredients] = useState([]);

    const commentsUrl = `https://localhost:63516/recipe/${id}/comments`;
    const [comments, setComments] = useState([]);

    const visitsUrl = `https://localhost:63516/recipe/${id}/addVisitor`;

    const ratingUrl = `https://localhost:63516/recipe/${id}/ratings`;

    const addVisitor = () => {
        fetch(visitsUrl, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
        })
        .catch(console.error);
    };

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
        fetchComments();
        addVisitor();
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
            <img
            src={recipe.imagePath}
            className="recipeImageLarge"
            onError={(e) => {
                e.target.onerror = null;
                e.target.src = missing_image;
            }}
            />
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
            <div className="recipeDescription" dangerouslySetInnerHTML={{ __html: recipe.description }} />
            <div className="recipeRating">
                <p>
                    Give this recipe a rating!
                </p>
                <RatingBlock recipeRating={recipe.avgRating} recipeId={recipe.id} onRatingChange={fetchRecipes} interactive={true} />
            </div>
            <div className="recipeComments">
                <Collapsible label="Comments">
                    <AddComment recipeId={recipe.id} onCommentChange={fetchComments} />
                    {comments.map((comment) => (
                        <CommentBlock comment={comment} key={comment.id} onCommentChange={fetchComments} />
                    ))}
                </Collapsible>
            </div>
            <div>
                <Link to={`/editRecipe/${recipe.id}`}>Edit Recipe</Link>
            </div>
        </div>
    )
};

export default Recipe;