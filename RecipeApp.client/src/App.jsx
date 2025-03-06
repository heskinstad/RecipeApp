import { useEffect, useState } from 'react'
import { Link, Route, Routes } from "react-router-dom";
import './App.css'
import Frontpage from './pages/frontpage/frontpage'

import { createContext } from 'react';
import Collapsible from './components/collapsible/collapsible';
import Categories from './pages/categories/categories';
const RecipeContext = createContext();

function App() {
  const categoriesUrl = "https://localhost:63516/category";
  const [categories, setCategories] = useState([]);

  const recipesUrl = "https://localhost:63516/recipe";
  const [recipes, setRecipes] = useState([]);

  const fetchCategories = () => {
    fetch(categoriesUrl)
    .then((res) => {
      return res.json();
    })
    .then((jsonData) => {
      setCategories(jsonData);
    })
  };

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
    fetchCategories();
    fetchRecipes();
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
          <Collapsible label="Categories">
            {categories.map((category) => (
              <Link to={`/category/${category.name}`.toLowerCase()}>
                <li className="category-link">{category.name}</li>
              </Link>
            ))}
          </Collapsible>
        </ul>
      </aside>

      <main>
        <RecipeContext.Provider value={{recipes, setRecipes }}>
          <Routes>
            <Route path="/" element={<Frontpage />} />
            <Route path="category/:name" element={<Categories />} />
            <Route path="/recipe/:name" element={<Recipe />} />
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