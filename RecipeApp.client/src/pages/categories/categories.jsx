import { useParams } from 'react-router-dom';
import './style.css';
import { RecipeContext } from '../../App';
import { useContext } from "react"

function Categories() {
    const { recipes } = useContext{RecipeContext};
    const { name } = useParams();

    return (
        <>
            <div className="content">
                <h1 className="capitalize">{ name }</h1>
            </div>
            <ul>
                {recipes.map((category) => (
                <Link to={`/category/${category.name}`.toLowerCase()}>
                    <li className="category-link">{category.name}</li>
                </Link>
                ))}
            </ul>
        </>
    )
};

export default Categories;