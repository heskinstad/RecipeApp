import { useParams } from 'react-router-dom';
import './style.css';

function Categories() {
    const {name} = useParams();

    return (
        <>
            <div className="content">
                <h1>{ name }</h1>
            </div>
            
        </>
    )
};

export default Categories;