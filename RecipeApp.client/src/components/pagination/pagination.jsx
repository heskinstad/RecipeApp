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

    const currentSelectValueSort = sortOptions.find(
        (option) => option.value === sortBy
    ) || null;

    const displayOptions = [
        { value: 'list', label: 'List' },
        { value: 'compact_list', label: 'Compact list' },
        { value: 'grid', label: 'Grid' }
    ];

    const currentSelectValueDisplay = displayOptions.find(
        (option) => option.value === 'list'
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
                    onChange={(selectedOption) => setDisplayMode(selectedOption ? selectedOption.value : '')}
                />
                <label className="pagination_controls_text">(Total results: {totalCount})</label>
            </div>
            <div>
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