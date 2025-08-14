import { useState } from 'react';
import './style.css';
import upvote_svg from "../../resources/buttons/upvote.svg"
import downvote_svg from "../../resources/buttons/downvote.svg"

function CommentBlock({comment}) {

    const [localComment, setLocalComment] = useState(comment);

    const date = new Date(comment.createdAt);

    const time = date.toLocaleTimeString([], {
    hour: "2-digit",
    minute: "2-digit",
    hour12: false
    });

    const datePart = date.toLocaleDateString("en-US", {
    year: "numeric",
    month: "long",
    day: "numeric"
    });

    const datetime = `${time} ${datePart}`;

    const upvoteUrl = `https://localhost:63516/userComment/${comment.id}/upvote`;
    const upvote = () => {
        fetch(upvoteUrl, {
            method: 'PUT',
        })
        .then(() => fetchComment())
        .catch((err) => console.log(err))
    }

    const downvoteUrl = `https://localhost:63516/userComment/${comment.id}/downvote`;
    const downvote = () => {
        fetch(downvoteUrl, {
            method: 'PUT',
        })
        .then(() => fetchComment())
        .catch((err) => console.log(err))
    }

    const fetchComment = () => {
        fetch(`https://localhost:63516/userComment/${comment.id}/`)
        .then((res) => {
          return res.json();
        })
        .then((jsonData) => {
          setLocalComment(jsonData)
        })
    };

    return (
        <div className="commentBlock">
            <div className="user">
                {localComment.userName} says:
            </div>
            <div className="date">
                {datetime}
            </div>
            <div className="comment">
                {localComment.message}
            </div>
            <div className="votes">
                <div className="upvotes" onClick={upvote}>
                    <img src={upvote_svg} alt="upvotes: " width="30" height="30" />
                    <h3>&nbsp;{localComment.upvotes}</h3>
                </div>
                <div className="downvotes" onClick={downvote}>
                    <img src={downvote_svg} alt="downvotes: " width="30" height="30" />
                    <h3>&nbsp;{localComment.downvotes}</h3>
                </div>
            </div>
        </div>
    );
}

export default CommentBlock;