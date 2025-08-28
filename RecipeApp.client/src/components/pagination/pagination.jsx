import React, { useState, useEffect } from 'react';

const Pagination = (url) => {
    const [data, setData] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(0);

    useEffect(() => {
        fetchData();
    }, [currentPage]);
    
    const fetchData = async () => {
        try {
            fetch(url)
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



    const fetchRecipes = () => {
        fetch(recipesUrl)
        .then((res) => {
          return res.json();
        })
        .then((jsonData) => {
          setRecipe(jsonData);
        })
    };

    return (
        <>
        </>
    )
}

export default Pagination;