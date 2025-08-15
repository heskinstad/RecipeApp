import { useState, useEffect } from 'react';
import './style.css';
import ImageSlider from '../../components/imageSlider/imageSlider';

function Frontpage() {
    const recipesUrl = `https://localhost:63516/recipe/randomMultiple?count=6`;
    const [recipes, setRecipe] = useState([]);

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
        <div className="content">
            <div className="header_recipe">
                <ImageSlider recipes={recipes} />
            </div>
            <div className="quote">
                <p>Cooking quote of some sorts...</p>
            </div>
        </div>
    )
};

export default Frontpage;