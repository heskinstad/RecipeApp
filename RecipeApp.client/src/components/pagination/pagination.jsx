import { useState, useEffect } from 'react';
import './pagination.css';

const Pagination = ({url, renderItem, searchString}) => {
    const [data, setData] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(0);
    const [pageSize, setPageSize] = useState(10);

    // Set current page to 1 when new search is made
    useEffect(() => {
        setCurrentPage(1);
    }, [searchString]);

    useEffect(() => {
        fetchData();
    }, [currentPage, searchString]);

    const fetchData = async () => {
        try {
            const response = await fetch(`${url}?pageNumber=${currentPage}&searchString=${encodeURIComponent(searchString)}`);
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

    return (
        <div>
            {data.map((item) => renderItem(item))}
            <div className="pagination_navigation">
                {Array.from({ length: totalPages }).map((_, index) => (
                    <button key={index + 1} onClick={() => goToPage(index + 1)}>{index + 1}</button>
                ))}
            </div>
        </div>
    )
}

export default Pagination;