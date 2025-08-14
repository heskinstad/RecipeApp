import HeaderRecipe from '../../components/headerRecipe/headerRecipe';
import './style.css';

function Frontpage() {
    return (
        <div className="content">
            <div className="header_recipe">
                <HeaderRecipe />
            </div>
            <div className="quote">
                <p>Cooking quote of some sorts...</p>
            </div>
        </div>
    )
};

export default Frontpage;