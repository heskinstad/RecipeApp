import Pagination from "../../components/pagination/pagination";

function Recipes() {
    const { keyword } = useParams();

    const recipesUrl = `https://localhost:63516/recipe/search?searchString=${keyword}`;
    const [recipes, setRecipes] = useState([]);

    const fetchRecipes = () => {
        fetch(recipesUrl)
        .then((res) => {
          return res.json();
        })
        .then((jsonData) => {
          setRecipes(jsonData);
        })
    };

    useEffect(() => {
        fetchRecipes();
    }, [keyword]);

    return (
        <div className="recipes_content">
            <h1 className="recipes_capitalize">{keyword}</h1>
            <Pagination
            url={recipesUrl}
            renderItem={(recipe) => <RecipeItem recipe={recipe} key={recipe.id} />}
            />
        </div>
    )
}

export default Recipes;