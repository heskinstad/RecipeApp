import { useState, useEffect } from 'react';
import RecipeItem from '../recipeItem/recipeItem';

const Pagination = (url, renderItem) => {
    const [data, setData] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(0);

    useEffect(() => {
        fetchData();
    }, [currentPage]);
    
    const fetchData = async () => {
        try {
            fetch(url + `?pageNumber=${currentPage}`)
            .then((res) => {
                return res.json();
            })
            .then((jsonData) => {
                setData(jsonData);
            })

            // Calculate total pages based on response
            setTotalPages(10); // Example: Assuming 10 total pages
        } catch (error) {
        console.error('Error fetching data:', error);
        }
    };

    const goToPage = (page) => {
        setCurrentPage(page);
    };

    return (
        <div>
            {data.map((item) => renderItem(item))}
            <div>
                {Array.from({ length: totalPages }).map((_, index) => (
                    <button key={index + 1} onClick={() => goToPage(index + 1)}>{index + 1}</button>
                ))}
            </div>
        </div>
    )
}

export default Pagination;