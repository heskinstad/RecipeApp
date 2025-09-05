import { useContext } from 'react';
import { RecipeContext } from '../../App';
import Pagination from "../../components/pagination/pagination";
import RecipeItem from "../../components/recipeItem/recipeItem";
import RecipeItemCompact from '../../components/recipeItemCompact/recipeItemCompact';
import RecipeItemGrid from '../../components/recipeItemGrid/recipeItemGrid';

function Recipes() {
    const { searchInput } = useContext(RecipeContext);
    const recipesUrl = `https://localhost:63516/recipe/search`;

    return (
        <>
            <div className="recipes_content">
                <Pagination
                    url={recipesUrl}
                    searchString={searchInput}
                    renderItem={(recipe) => <RecipeItem recipe={recipe} key={recipe.id} />}
                />
            </div>
        </>
    )
}

export default Recipes;