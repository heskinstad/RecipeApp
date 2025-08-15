import React, { useState, useEffect } from 'react';
import './style.css';
import arrow_left from "../../resources/buttons/arrow_left.png"
import arrow_right from "../../resources/buttons/arrow_right.png"

const ImageSlider = ({ recipes }) => {
  const [current, setCurrent] = useState(0);
  const length = recipes.length;

  const nextSlide = () => {
    setCurrent(current === length - 1 ? 0 : current + 1);
  };

  const prevSlide = () => {
    setCurrent(current === 0 ? length - 1 : current - 1);
  };

  // Automatically shift to the next image every 5th second
  useEffect(() => {
    const interval = setInterval(() => {
      nextSlide();
    }, 5000); // Change slide every 5 seconds

    return () => clearInterval(interval);
  }, [current]);

  if (!Array.isArray(recipes) || recipes.length <= 0) {
    return null;
  }

  return (
    <section className='slider'>
      {/* <FaArrowAltCircleLeft className='left-arrow' onClick={prevSlide} />
      <FaArrowAltCircleRight className='right-arrow' onClick={nextSlide} /> */}
      <img src={arrow_left} className='left-arrow' onClick={prevSlide} />
      <img src={arrow_right} className='right-arrow' onClick={nextSlide} />
      {recipes.map((recipe, index) => {
        return (
          <div
            className={index === current ? 'slide active' : 'slide'}
            key={recipe.id} // Use id as key for better performance
          >
            {index === current && (
              <div className='slide-content'>
                <img src={recipe.imagePath} alt={recipe.name} className='image' />
                <div className='text-content'>
                  <h2>{recipe.name}</h2>
                </div>
              </div>
            )}
          </div>
        );
      })}
    </section>
  );
};

export default ImageSlider;
