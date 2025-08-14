import "./style.css";
import { useEffect, useState } from 'react';

function HeaderRecipe() {
    const recipesUrl = `https://localhost:63516/recipe/randomMultiple?count=6`;
    const [recipe, setRecipe] = useState([]);

    const fetchRecipes = () => {
        fetch(recipesUrl)
        .then((res) => {
          return res.json();
        })
        .then((jsonData) => {
          setRecipe(jsonData);
        })
    };

    useEffect(() => {
        fetchRecipes();
    }, []);

    return (
        <p>{recipe.map(r => r.name).join(", ")}</p>
    )
};

export default HeaderRecipe;