import { useState, useEffect } from 'react';
import { Link } from "react-router-dom";
import './frontpage.css';
import ImageSlider from '../../components/imageSlider/imageSlider';

function Frontpage() {
    const recipesUrl = `https://localhost:63516/recipe/randomMultiple?count=6`;
    const [recipes, setRecipe] = useState([]);

    const categoriesUrl = "https://localhost:63516/category";
    const [categories, setCategories] = useState([]);

    const fetchRecipes = () => {
        fetch(recipesUrl)
        .then((res) => {
          return res.json();
        })
        .then((jsonData) => {
          setRecipe(jsonData);
        })
    };

    const fetchCategories = () => {
        fetch(categoriesUrl)
        .then((res) => {
        return res.json();
        })
        .then((jsonData) => {
        setCategories(jsonData);
        })
    };

    useEffect(() => {
        fetchRecipes();
        fetchCategories();
    }, []);

    return (
        <div className="frontpage_content">
            <div className="frontpage_header-recipe">
                <ImageSlider recipes={recipes} />
            </div>
            <div className="frontpage_quote">
                <p>Cooking quote of some sorts...</p>
            </div>
            <div className="frontpage_all-recipes">
                <Link to="/recipes">
                    <h2>All Recipes</h2>
                </Link>
            </div>
            <div className="frontpage_categories">
                <h2>Categories</h2>
                <div className="frontpage_categories-list">
                    {categories.map((category) => (
                    <Link to={`/category/${category.name}`.toLowerCase()} key={category.name}>
                        <div className="frontpage_category-list-item">
                            <h3>{category.name}</h3>
                        </div>
                    </Link>
                    ))}
                </div>
            </div>
            <div className="frontpage_new-recipe">
                <Link to="/addrecipe">
                    <h2>Add new recipe</h2>
                </Link>
            </div>
        </div>
    )
};

export default Frontpage;