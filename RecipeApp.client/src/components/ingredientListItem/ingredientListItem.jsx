function IngredientListItem({ingredient}) {

    return (
        <p>
            {ingredient.ingredientName} {ingredient.amount} {ingredient.unitName}
        </p>
    )
};

export default IngredientListItem;