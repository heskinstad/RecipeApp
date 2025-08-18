import React, { useState, useEffect } from 'react';
import { Link } from "react-router-dom";
import './style.css';
import arrow_left from "../../resources/buttons/arrow_left.png"
import arrow_right from "../../resources/buttons/arrow_right.png"

const ImageSlider = ({ recipes }) => {
  const [current, setCurrent] = useState(0);
  const [prev, setPrev] = useState(null);
  const [direction, setDirection] = useState('next');
  const length = recipes.length;

  const nextSlide = () => {
    setPrev(current);
    setCurrent(current === length - 1 ? 0 : current + 1);
    setDirection('next');
  };

  const prevSlide = () => {
    setPrev(current);
    setCurrent(current === 0 ? length - 1 : current - 1);
    setDirection('prev');
  };

  // Automatically shift to the next image every 5th second
  useEffect(() => {
    const interval = setInterval(() => {
      nextSlide();
    }, 8000); // Change slide every 8 seconds

    return () => clearInterval(interval);
  }, [current]);

  if (!Array.isArray(recipes) || recipes.length <= 0) {
    return null;
  }

  return (
    <section className='slider'>
      <img src={arrow_left} className='left-arrow' onClick={prevSlide} />
      <img src={arrow_right} className='right-arrow' onClick={nextSlide} />

      {recipes.map((recipe, index) => {
        let className = 'slide';
        if (index === current) {
          className += direction === 'next' ? ' slide-in-from-right' : ' slide-in-from-left';
        } else if (index === prev) {
          className += direction === 'next' ? ' slide-out-to-left' : ' slide-out-to-right';
        }

        return (
          <Link to={`/recipe/${recipe.id}`.toLowerCase()}>
            <div className={className} key={recipe.id}>
              <div className='text-content'>
                <h2>{recipe.name}</h2>
              </div>
              <div>
                <img src={recipe.imagePath} alt={recipe.name} className='image' />
              </div>
            </div>
          </Link>
        );
      })}
    </section>
  );
};

export default ImageSlider;
