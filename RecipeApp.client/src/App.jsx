import { useEffect, useState } from 'react'
import { Link, Route, Routes } from "react-router-dom";
import './App.css'
import Frontpage from './pages/frontpage/frontpage'

import { createContext } from 'react';
import Collapsible from './components/collapsible/collapsible';
import Categories from './pages/categories/categories';
import Recipe from './pages/recipe/recipe';
import AllRecipes from './pages/allRecipes/allRecipes';
import AddRecipe from './pages/addRecipe/addRecipe';
const RecipeContext = createContext();

function App() {
  const categoriesUrl = "https://localhost:63516/category";
  const [categories, setCategories] = useState([]);

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
    fetchCategories();
  }, []);

  return (
    <div className="app">
      <header>
        <form>
          <input>
          </input>
        </form>
      </header>

      <aside>
        <div className="aside-header">
          <h1>Recipe App</h1>
        </div>
        <ul className="aside-categories">
          <Link to="/">
            <li className="category-link">Frontpage</li>
          </Link>
          <Link to="/recipes">
            <li className="category-link">All recipes</li>
          </Link>
          <Collapsible label="Categories">
            {categories.map((category) => (
              <Link to={`/category/${category.name}`.toLowerCase()} key={category.name}>
                <li className="category-link">{category.name}</li>
              </Link>
            ))}
          </Collapsible>
        </ul>
      </aside>

      <main>
        <RecipeContext.Provider value={{}}>
          <Routes>
            <Route path="/" element={<Frontpage />} />
            <Route path="category/:name" element={<Categories />} />
            <Route path="recipe/:id" element={<Recipe />} />
            <Route path="recipes" element={<AllRecipes />} />
            <Route path="addRecipe" element={<AddRecipe />} />
          </Routes>
        </RecipeContext.Provider>
      </main>

      <footer>

      </footer>
    </div>
  )
}

export { RecipeContext };
export default App;