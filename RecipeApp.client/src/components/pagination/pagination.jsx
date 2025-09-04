import { useState, useEffect } from 'react';
import './pagination.css';

const Pagination = ({url, renderItem, searchString}) => {
    const [data, setData] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(0);
    const [pageSize, setPageSize] = useState(10);
    const [sortBy, setSortBy] = useState('name');

    // Set current page to 1 when new search is made
    useEffect(() => {
        setCurrentPage(1);
    }, [searchString]);

    useEffect(() => {
        fetchData();
    }, [currentPage, searchString, sortBy]);

    const fetchData = async () => {
        try {
            const response = await fetch(`${url}?pageNumber=${currentPage}&searchString=${encodeURIComponent(searchString)}&sortBy=${sortBy}`);
            const jsonData = await response.json();

            const { items, totalCount, pageSize } = jsonData;

            setData(items);
            setPageSize(pageSize);
            setTotalPages(Math.ceil(totalCount / pageSize));

        } catch (error) {
            console.error('Error fetching data:', error);
        }
    };

    const goToPage = (page) => {
        setCurrentPage(page);
    };

    const handleChange = (e) => {
        setSortBy(e.target.value);
    }

    return (
        <div>
            <div>
                <label>Sort by: </label>
                <select value={sortBy} onChange={handleChange}>
                    <option value="date">Date</option>
                    <option value="date_desc">Date descending</option>
                    <option value="name">Name</option>
                    <option value="name_desc">Name descending</option>
                    <option value="rating">Rating</option>
                    <option value="rating_desc">Rating descending</option>
                </select>
                {data.map((item) => renderItem(item))}
                <div className="pagination_navigation">
                    {Array.from({ length: totalPages }).map((_, index) => (
                        <button key={index + 1} onClick={() => goToPage(index + 1)}>{index + 1}</button>
                    ))}
                </div>
            </div>
        </div>
    )
}

export default Pagination;