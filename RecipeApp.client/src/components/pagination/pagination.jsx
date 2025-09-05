import { useState, useEffect } from 'react';
import './pagination.css';

const Pagination = ({url, renderItem, searchString}) => {
    const [data, setData] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(0);
    const [pageSize, setPageSize] = useState(8);
    const [sortBy, setSortBy] = useState('date_desc');
    const [totalCount, setTotalCount] = useState(0);

    // Set current page to 1 when new search is made
    useEffect(() => {
        setCurrentPage(1);
    }, [searchString, sortBy]);

    useEffect(() => {
        fetchData();
    }, [currentPage, searchString, sortBy]);

    const fetchData = async () => {
        try {
            const response = await fetch(`${url}?pageNumber=${currentPage}&searchString=${encodeURIComponent(searchString)}&sortBy=${sortBy}`);
            const jsonData = await response.json();
            
            setTotalCount(jsonData.totalCount);
            const { items, totalCount, pageSize } = jsonData;

            setData(items);
            setTotalPages(Math.ceil(totalCount / pageSize));

        } catch (error) {
            console.error('Error fetching data:', error);
        }
    };

    const goToPage = (page) => {
        setCurrentPage(page);
    };

    const handleSortChange = (e) => {
        setSortBy(e.target.value);
    }

    // const handleDisplayModeChange = (e) => {
    //     setPageSize(e.target.value === 'grid' ? 16 : e.target.value === 'compact_list' ? 16 : 6);
    // }

    return (
        <div>
            <div>
                <label>Sort by: </label>
                <select value={sortBy} onChange={handleSortChange}>
                    <option value="date">Date</option>
                    <option value="date_desc">Date descending</option>
                    <option value="name">Name</option>
                    <option value="name_desc">Name descending</option>
                    <option value="rating">Rating</option>
                    <option value="rating_desc">Rating descending</option>
                </select>
                <label>Display mode: </label>
                {/* <select value={displayMode} onChange={handleDisplayModeChange}> */}
                <select>
                    <option value="list">List</option>
                    <option value="compact_list">Compact list</option>
                    <option value="grid">Grid</option>
                </select>
                <label> (Total results: {totalCount})</label>

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