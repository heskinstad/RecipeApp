import './style.css';
import { Link} from "react-router-dom";

function RecipeItem({recipe}) {
    return (
        <>
            <div className="upperDiv">
                <h1>{recipe.name}</h1>
                <Link to={`/recipe/${recipe.id}`.toLowerCase()}>
                    tee
                </Link>
            </div>
            <hr />
        </>
    )
};

export default RecipeItem;