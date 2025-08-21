import { useParams } from 'react-router-dom';
import './categories.css';
import { useEffect, useState } from 'react'
import RecipeItem from '../../components/recipeItem/recipeItem';

function Categories() {
    const { name } = useParams();

    const recipesUrl = `https://localhost:63516/recipe/category?name=${name}`;
    const [recipes, setRecipes] = useState([]);

    const fetchRecipes = () => {
        fetch(recipesUrl)
        .then((res) => {
          return res.json();
        })
        .then((jsonData) => {
          setRecipes(jsonData);
        })
    };

    useEffect(() => {
        fetchRecipes();
    }, [name]);

    return (
        <>
            <div className="category_content">
                <h1 className="capitalize">{ name }</h1>
            </div>
            {recipes.map((recipe) => (
                <RecipeItem recipe={recipe} key={recipe.id} />
            ))}
        </>
    )
};

export default Categories;