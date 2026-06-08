import { useState, useEffect } from 'react';
import Select from 'react-select';
import './pagination.css';

const Pagination = ({url, renderItem, searchString}) => {
    const [data, setData] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(0);
    const [pageSize, setPageSize] = useState(8);
    const [sortBy, setSortBy] = useState('date_desc');
    const [totalCount, setTotalCount] = useState(0);
    const [displayMode, setDisplayMode] = useState('cards'); 

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
            const { items, totalCount, pageSize: apiPageSize } = jsonData;

            setData(items);
            setTotalPages(Math.ceil(totalCount / (apiPageSize || pageSize)));

        } catch (error) {
            console.error('Error fetching data:', error);
        }
    };

    const goToPage = (page) => {
        setCurrentPage(page);
    };

    const sortOptions = [
        { value: 'date', label: 'Date' },
        { value: 'date_desc', label: 'Date descending' },
        { value: 'name', label: 'Name' },
        { value: 'name_desc', label: 'Name descending' },
        { value: 'rating_desc', label: 'Highest rating' },
        { value: 'rating', label: 'Lowest rating' },
        { value: 'visits_desc', label: 'Most visited' },
        { value: 'visits', label: 'Least visited' }
    ];

    const displayOptions = [
        { value: 'list', label: 'List' },
        { value: 'compact_list', label: 'Compact list' },
        { value: 'cards', label: 'Cards' }
    ];

    const currentSelectValueSort = sortOptions.find(
        (option) => option.value === sortBy
    ) || null;

    const currentSelectValueDisplay = displayOptions.find(
        (option) => option.value === displayMode
    ) || null;

    return (
        <div>
            <div className="pagination_controls">
                <label className="pagination_controls_text">Sort by:</label>
                <Select
                    className="pagination_sortSelect"
                    options={sortOptions}
                    value={currentSelectValueSort}
                    onChange={(selectedOption) => setSortBy(selectedOption ? selectedOption.value : '')}
                />
                <label className="pagination_controls_text">Display mode:</label>
                <Select
                    className="pagination_displaySelect"
                    options={displayOptions}
                    value={currentSelectValueDisplay}
                    onChange={(selectedOption) => setDisplayMode(selectedOption ? selectedOption.value : 'cards')}
                />
                <label className="pagination_controls_text">(Total results: {totalCount})</label>
            </div>
            
            <div className={`pagination_items_container ${displayMode}`}>
                {data.map((item) => renderItem(item))}
            </div>

            <div className="pagination_navigation">
                {Array.from({ length: totalPages }).map((_, index) => (
                    <button key={index + 1} onClick={() => goToPage(index + 1)}>{index + 1}</button>
                ))}
            </div>
        </div>
    )
}

export default Pagination;
