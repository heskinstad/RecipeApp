import { useState } from 'react';
import Collapsible from "../collapsible/collapsible";
import "./addComment.css";

function AddComment({recipeId, onCommentChange}) {
    const [formData, setFormData] = useState({
            userId: "0198a3ae-a9d7-7dfe-9031-12e95619f54f",
            recipeId: recipeId,
            message: "",
            upvotes: 0,
            downvotes: 0,
    });

    const [collapsibleOpen, setCollapsibleOpen] = useState(false);

    const commentUrl = `https://localhost:63516/recipe/${recipeId}/comments`;

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        fetch(commentUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(formData),
        })
        .then (() => {
            setFormData({ ...formData, message: "" });
            onCommentChange();

            // then collapse the collapsible:
            setCollapsibleOpen(false);
        })
        .catch(console.error);
    }

    return (
        <div className="addComment_upper-div">
            <Collapsible
            label="Add new comment"
            open={collapsibleOpen}
            setOpen={setCollapsibleOpen}
            >
                <div className="addComment_div">
                    <form onSubmit={handleSubmit}>
                        <div className="addComment_inner-div">
                            <label>
                                <textarea
                                    className="addComment_inner-div-textarea"
                                    name="message"
                                    onChange={handleChange}
                                    value={formData.message}
                                />
                            </label>
                        </div>
                        <div className="addComment_div-submit">
                            <input type="submit" value="Post comment" />
                        </div>
                    </form>
                </div>
            </Collapsible>
        </div>
    )
}

export default AddComment;