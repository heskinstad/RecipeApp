import { useParams } from 'react-router-dom';
import './style.css';
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
            <div className="content">
                <h1 className="capitalize">{ name }</h1>
            </div>
            {recipes.map((recipe, index) => (
                <RecipeItem recipe={recipe} key={index} />
            ))}
        </>
    )
};

export default Categories;